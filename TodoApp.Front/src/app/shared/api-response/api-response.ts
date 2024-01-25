export interface APIResponse<T>{
    data: T;
    messages: string[];
    isSuccess: boolean;
    itemsPerPage: number;
    selectedPage: number;
    itemsCount: number;
}