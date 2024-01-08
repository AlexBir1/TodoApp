export interface AuthorizationModel{
    accountId: string;
    token: string;
    tokenExpirationDate: Date;
    keepAuthorized: boolean;
}