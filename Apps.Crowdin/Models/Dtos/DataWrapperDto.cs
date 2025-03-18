namespace Apps.Crowdin.Models.Dtos;

public class DataWrapperDto<T>
{
    public T Data { get; set; } = default!;
}
