export interface BookFileUploadData {
    title: string,
    maxFileSize: number,
    allowMultiple: boolean,
    allowedFileTypes: string[],
    uploaderText: string,
    fileHint: string
}