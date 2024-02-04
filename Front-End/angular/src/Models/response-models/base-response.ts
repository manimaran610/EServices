export interface BaseResponse<T> {
    succeeded: boolean;
    message: string;
    errors: string[];
    data: T;
    totalCount:number;
    resultCount:number;
}