import axios, { AxiosInstance } from 'axios';
import { FilesApi, UsersApi } from '../../api/api';
import { Configuration } from '../../api/configuration';

export class ApiClient {
    constructor() {
        const configuration: Configuration = new Configuration({});
        const axiosInstance = this.getAxiosInstance();
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
