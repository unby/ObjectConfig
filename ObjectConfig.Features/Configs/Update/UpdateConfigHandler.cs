using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ObjectConfig.Features.Configs.Update
{
    public class UpdateConfigHandler : IRequestHandler<UpdateConfigCommand, Config>
    {
        private readonly ObjectConfigContext _configContext;
        private readonly ConfigService _configService;
        private readonly ILogger<UpdateConfigHandler> _logger;

        public UpdateConfigHandler(ObjectConfigContext configContext, ConfigService configService, ILogger<UpdateConfigHandler> logger)
        {
            _configContext = configContext;
            _configService = configService;
            _logger = logger;
        }

        public async Task<Config> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
        {
            var configSource = await _configService.GetConfigElement(
                () => _configService.GetConfig(request, EnvironmentRole.Editor, cancellationToken), cancellationToken);

            Config config = new Config(request.ConfigCode, configSource.config.Environment, request.VersionFrom);
            var reader = new ObjectConfigReader(config);
            ConfigElement configElement = await reader.Parse(request.Data);

            var comparer = new ElementComparer(configSource.root, configElement, _logger);
            comparer.Compare();
            _logger.LogDebug("count change: {0}",await _configContext.SaveChangesAsync(cancellationToken));
            _configContext.ConfigCache.Update(new ConfigCache(config, request.Data));
            return configSource.config;
        }
    }

    public class ElementComparer
    {
        private readonly ConfigElement _sourceElement;
        private readonly ConfigElement _newElement;
        private readonly ILogger<UpdateConfigHandler> _logger;
        public readonly DateTimeOffset CloseDate = DateTimeOffset.Now;

        public ElementComparer(ConfigElement sourceElement, ConfigElement newElement,
            ILogger<UpdateConfigHandler> logger)
        {
            _sourceElement = sourceElement;
            _newElement = newElement;
            _logger = logger;
        }

        public void Compare()
        {
            Compare(_sourceElement, _newElement);
        }

        public void Compare(ConfigElement sourceElementParrent, ConfigElement newElementParrent)
        {
            try
            {
                if(sourceElementParrent.Path.Contains("SimpleArray"))
                    _logger.LogDebug(sourceElementParrent.Path);
                List<int> oldElements = new List<int>(newElementParrent.Childs.Count);
                List<(ConfigElement oldArray, ConfigElement newArray)> deletedArrays =
                    new List<(ConfigElement oldArray, ConfigElement newArray)>();
                foreach (var sourceElementChild in sourceElementParrent.Childs)
                {
                    int newElementIndex = -1;
                    for (int i = 0; i < newElementParrent.Childs.Count(); i++)
                    {
                        if (sourceElementChild.Path.Equals(newElementParrent.Childs[i].Path))
                        {
                            newElementIndex = i;
                            break;
                        }
                    }

                    oldElements.Add(newElementIndex);

                    if (newElementIndex < 0)
                    {
                        Delete(sourceElementChild);
                    }
                    else
                    {
                        var newElementChild = newElementParrent.Childs[newElementIndex];

                        if (sourceElementChild.TypeElement.Equals(newElementChild.TypeElement) &&
                            newElementChild.TypeElement.Type == TypeNode.Complex)
                        {
                            Compare(sourceElementChild, newElementChild);
                            continue;
                        }
                        else if (sourceElementChild.TypeElement.Type.Equals(newElementChild.TypeElement.Type) &&
                                 newElementChild.TypeElement.Type == TypeNode.Array)
                        {
                            deletedArrays.Add((sourceElementChild, newElementChild));

                            continue;
                        }
                        else   if(sourceElementChild.TypeElement.Type.Equals(TypeNode.Root))
                            continue;
                        else
                        {
                            try
                            {

                                if (!sourceElementChild.TypeElement.Type.Equals(newElementChild.TypeElement.Type))
                                {
                                    foreach (var sourceValue in sourceElementChild.Value)
                                    {
                                        sourceValue.Close(CloseDate);
                                    }

                                    foreach (var valueElement in newElementChild.Value)
                                    {
                                        sourceElementChild.Value.Add(new ValueElement(valueElement.Value,
                                            sourceElementChild, CloseDate));
                                    }
                                }
                                else if (!sourceElementChild.Value[0].Value.Equals(newElementChild.Value[0].Value))
                                {
                                    foreach (var sourceValue in sourceElementChild.Value)
                                    {
                                        sourceValue.Close(CloseDate);
                                    }

                                    foreach (var valueElement in newElementChild.Value)
                                    {
                                        sourceElementChild.Value.Add(new ValueElement(valueElement.Value,
                                            sourceElementChild, CloseDate));
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                _logger.LogDebug("{0} {1}", sourceElementParrent.Path, ex);
                                throw;
                            }
                        }
                    }
                }

                foreach (var item in deletedArrays)
                {
                    Delete(item.oldArray);
                    New(sourceElementParrent, item.newArray);
                }

                for (int i = 0; i < newElementParrent.Childs.Count(); i++)
                {
                    if(!oldElements.Contains(i))
                        New(sourceElementParrent, newElementParrent.Childs[i]);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("{0} {1}", sourceElementParrent.Path, ex);
                throw new Exception( sourceElementParrent.Path, ex);
            }
        }

        private void New(ConfigElement sourceElement, ConfigElement newElement)
        {
            newElement.CopyTo(sourceElement.Config);
            if (newElement.Parrent != sourceElement)
            {
                sourceElement.Childs.Add(newElement);
            }

            foreach (var child in newElement.Childs)
            {
                New(newElement, child);
            }
        }

        private void Delete(ConfigElement sourceElementChild)
        {
            foreach (var val in sourceElementChild.Value)
            {
                val.Close(CloseDate);
            }

            foreach (var childElement in sourceElementChild.Childs)
            {
                Delete(childElement);
            }
        }
    }
}
