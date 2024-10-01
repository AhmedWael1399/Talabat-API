namespace TalabatApi.Extensions
{
    public static class AddSwaggerExtension
    {
        public static WebApplication UseSwaggerMiddleWare(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
