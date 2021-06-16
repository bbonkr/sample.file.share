import { AxiosError } from 'axios';
import { ActionType, createAction, createAsyncAction } from 'typesafe-actions';
import {
    ApiResponseModel,
    CreateUserCommand,
    UserModelApiResponseModel,
    UserModelIPagedModelApiResponseModel,
} from '../../../api';

import {
    DeleteUserByIdRequest,
    GetUserByEmailRequest,
    GetUsersRequest,
} from '../../models/users';

const getUserByEmail = createAsyncAction(
    'get-user-by-email/request',
    'get-user-by-email/success',
    'get-user-by-email/failure',
)<GetUserByEmailRequest, UserModelApiResponseModel, ApiResponseModel>();

const createUser = createAsyncAction(
    'create-user/request',
    'create-user/success',
    'create-user/failure',
)<CreateUserCommand, UserModelApiResponseModel, ApiResponseModel>();

const deleteUser = createAsyncAction(
    'delete-user/request',
    'delete-user/success',
    'delete-user/failure',
)<DeleteUserByIdRequest, ApiResponseModel, ApiResponseModel>();

const getUsers = createAsyncAction(
    'get-users/request',
    'get-users/success',
    'get-users/failure',
)<GetUsersRequest, UserModelIPagedModelApiResponseModel, ApiResponseModel>();

export const clearUser = createAction('clear-user')();
export const clearUsers = createAction('clear-users')();
const clearError = createAction('clear-error')();

export const userActions = {
    getUserByEmail,
    createUser,
    deleteUser,
    getUsers,
    clearUser,
    clearUsers,
    clearError,
};

export type UserActions = ActionType<typeof userActions>;
