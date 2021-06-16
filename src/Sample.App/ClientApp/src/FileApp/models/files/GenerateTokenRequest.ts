import { ApiResponseModel, GenerateTokenRequest } from '../../../api';
import { PagedModelRequestBase } from '../PagedModelRequestBase';
import { AuthorizedRequest } from '../AuthorizedRequest';

export type GetFilesRequest = PagedModelRequestBase & AuthorizedRequest;

export type UploadFileRequest = AuthorizedRequest & {
    files: File[];
};

export type ShareFileRequest = AuthorizedRequest & {
    fileId: string;
    generateTokenRequest?: GenerateTokenRequest;
};

export type DeleteFileRequest = AuthorizedRequest & {
    fileId: string;
};

export type DeleteFileResponse = ApiResponseModel & {
    fileId: string;
};
