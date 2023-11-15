import { BaseResponse } from "./base-response";

export interface PagedResponse<T> extends BaseResponse<T> {
    offset: number;
    count: number;
    totalCount: number;
    resultCount: number;
}