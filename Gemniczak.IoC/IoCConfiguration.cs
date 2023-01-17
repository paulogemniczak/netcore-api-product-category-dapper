namespace Gemniczak.IoC
{
  public static class IoCConfiguration
  {
    public static Dictionary<Type, Type> GetDataTypes()
    {
      return Data.IoC.Module.GetTypes();
    }

    public static Dictionary<Type, Type> GetAppServiceTypes()
    {
      return AppService.IoC.Module.GetTypes();
    }
  }
}
