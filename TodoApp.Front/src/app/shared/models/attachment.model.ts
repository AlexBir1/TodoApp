export interface AttachmentModel{
    id: string;
    filename: string;
    extension: string;
    contentType: string;
    fullpath: string;
    size: number;
    
    goalId: string;
}