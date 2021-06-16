import axios, { AxiosError, AxiosInstance } from 'axios';
import { ApiResponseModel, FilesApi, UsersApi } from '../../api/api';
import { Configuration } from '../../api/configuration';

export class ApiClient {
    constructor() {
        const configuration: Configuration = new Configuration({});
        const axiosInstance = this.getAxiosInstance();
        axiosInstance.interceptors.response.use(
            (res) => res,
            (err) => {
                if (axios.isAxiosError(err)) {
                    const axiosErr = err as AxiosError<ApiResponseModel>;

                    throw axiosErr.response?.data;
                }
                throw err;
            },
        );
        this.files = new FilesApi(configuration, '', axiosInstance);
        this.users = new UsersApi(configuration, '', axiosInstance);
    }

    public readonly files: FilesApi;
    public readonly users: UsersApi;

    private getAxiosInstance(): AxiosInstance {
        const instance = axios.create();

        return instance;
    }
}
