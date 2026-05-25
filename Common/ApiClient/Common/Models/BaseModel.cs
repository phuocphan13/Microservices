namespace ApiClient.Common.Models;

public class BaseModel<T>
    where T : struct
{
    public T Id { get; set; }
}