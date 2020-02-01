using ObjectConfig.Data;

namespace UnitTests.Data
{
    public class DataSeed
    {
        public static User UserAdmin1 => new User(2, "admin1", "admin1", "admin1@test.test", User.Role.Administrator);

        public static User UserViewer1 => new User(3, "viewer1", "viewer1", "viewer1@test.test", User.Role.Viewer);

        public static User UserAdmin2 => new User(4, "admin2", "admin2", "admin2@test.test", User.Role.Administrator);

        public static User UserViewer2 => new User(5, "viewer2", "viewer2", "viewer2@test.test", User.Role.Viewer);

        public static User UserAdmin3 => new User(6, "admin3", "admin3", "admin3@test.test", User.Role.Administrator);

        public static User UserViewer3 => new User(7, "viewer3", "viewer3", "viewer3@test.test", User.Role.Viewer);


        public static Application Application1 => new Application(1, "Application1", "Application1", "Application1");

        public static Application Application2 => new Application(2, "Application2", "Application2", "Application2");

        public static Application Application3 => new Application(3, "Application3", "Application3", "Application3");
    }
}
