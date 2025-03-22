namespace ManagementPias.App.Interfaces;

public interface IModelHelper
{
    string GetModelFields<T>();

    string ValidateModelFields<T>(string fields);
}