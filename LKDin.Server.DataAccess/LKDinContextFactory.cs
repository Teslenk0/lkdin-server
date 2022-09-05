namespace LKDin.Server.DataAccess
{
    public static class DbContextFactory
    {
        public static LKDinContext Create()
        {
            return new LKDinContext();
        }
    }
}
