using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using ObjectConfig.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Storage;

namespace ObjectConfig.Features.Configs.Update
{
    public class UpdateConfigHandler : IRequestHandler<UpdateConfigCommand, Config>
    {
        private readonly ObjectConfigContext _configContext;
        private readonly ConfigService _configService;
        private readonly CacheService _cacheService;
        private readonly ILogger<UpdateConfigHandler> _logger;
        private readonly DateTimeOffset _closeDate = DateTimeOffset.Now;

        public UpdateConfigHandler(ObjectConfigContext configContext,
            ConfigService configService,
            CacheService cacheService,
            ILogger<UpdateConfigHandler> logger)
        {
            _configContext = configContext;
            _configService = configService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Config> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction = _configContext.Database.BeginTransaction();
            try
            {
                var configSource = await _configService.GetConfigElement(
                    () => _configService.GetConfig(request, EnvironmentRole.Editor, cancellationToken),
                    cancellationToken);

                Config config = new Config(request.ConfigCode, configSource.config.Environment,
                    request.VersionFrom);
                var reader = new ObjectConfigReader(config);
                ConfigElement configElement = await reader.Parse(request.Data);
                Compare(configSource.root, configElement);

                await _configContext.SaveChangesAsync(cancellationToken);

                await _cacheService.UpdateJsonConfig(configSource.config.ConfigId, request.Data, cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return configSource.config;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "{request} data:{Data}", request, request.Data);
                throw;
            }
        }

        public void Compare(ConfigElement sourceElementParrent, ConfigElement newElementParrent)
        {
            var oldElements = new List<int>(newElementParrent.Childs.Count);
            var deletedArrays = new List<(ConfigElement oldArray, ConfigElement newArray)>();

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

                    if (sourceElementChild.TypeElement.TypeNode.Equals(newElementChild.TypeElement.TypeNode) &&
                        newElementChild.TypeElement.TypeNode == TypeNode.Complex)
                    {
                        Compare(sourceElementChild, newElementChild);
                        continue;
                    }
                    else if (sourceElementChild.TypeElement.TypeNode.Equals(newElementChild.TypeElement.TypeNode) &&
                             newElementChild.TypeElement.TypeNode == TypeNode.Array)
                    {
                        deletedArrays.Add((sourceElementChild, newElementChild));

                        continue;
                    }
                    else if (sourceElementChild.TypeElement.TypeNode.Equals(TypeNode.Root))
                        continue;
                    else
                    {
                        if (!sourceElementChild.TypeElement.TypeNode.Equals(newElementChild.TypeElement.TypeNode))
                        {
                            sourceElementChild.Close(_closeDate);
                            foreach (var sourceValue in sourceElementChild.Value)
                            {
                                sourceValue.Close(_closeDate);
                            }

                            foreach (var valueElement in newElementChild.Value)
                            {
                                sourceElementChild.Value.Add(
                                    new ValueElement(valueElement.Value, sourceElementChild, _closeDate));
                            }

                            oldElements.RemoveAt(oldElements.Count - 1);
                        }
                        else if ((sourceElementChild.Value[0].Value!=null && !sourceElementChild.Value[0].Value.Equals(newElementChild.Value[0].Value)))
                        {
                            foreach (var sourceValue in sourceElementChild.Value)
                            {
                                sourceValue.Close(_closeDate);
                            }

                            foreach (var valueElement in newElementChild.Value)
                            {
                                sourceElementChild.Value.Add(
                                    new ValueElement(valueElement.Value, sourceElementChild, _closeDate));
                            }
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
                if (!oldElements.Contains(i))
                    New(sourceElementParrent, newElementParrent.Childs[i]);
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
            sourceElementChild.Close(_closeDate);
            foreach (var val in sourceElementChild.Value)
            {
                val.Close(_closeDate);
            }

            foreach (var childElement in sourceElementChild.Childs)
            {
                Delete(childElement);
            }
        }
    }
}
