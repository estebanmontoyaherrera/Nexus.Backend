namespace SharedKernel.Commons.Bases;

public class BaseResponse<T> : BaseResponseToken
{
    public bool? IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public int? TotalRecords { get; set; }
    public IEnumerable<BaseError>? Errors { get; set; }
}

public class BaseResponseToken
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}