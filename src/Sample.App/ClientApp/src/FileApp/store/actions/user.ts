import { ActionType, createAction, createAsyncAction } from 'typesafe-actions';
import {
    ApiResponseModel,
    CreateUserCommand,
    CreateUserResultApiResponseModel,
    FindByEmailResultApiResponseModel,
    UserModelIPagedModelApiResponseModel,
} from '../../../api';

import {
    DeleteUserByIdRequest,
    GetUserByEmailRequest,
    GetUsersRequest,
} from '../../models/users';

export const getUserByEmail = createAsyncAction(
    'get-user-by-email/request',
    'get-user-by-email/success',
    'get-user-by-email/failure',
)<GetUserByEmailRequest, FindByEmailResultApiResponseModel, ApiResponseModel>();

export const createUser = createAsyncAction(
    'create-user/request',
    'create-user/success',
    'create-user/failure',
)<CreateUserCommand, CreateUserResultApiResponseModel, ApiResponseModel>();

export const deleteUser = createAsyncAction(
    'delete-user/request',
    'delete-user/success',
    'delete-user/failure',
)<DeleteUserByIdRequest, ApiResponseModel, ApiResponseModel>();

export const getUsers = createAsyncAction(
    'get-users/request',
    'get-users/success',
    'get-users/failure',
)<GetUsersRequest, UserModelIPagedModelApiResponseModel, ApiResponseModel>();

export const clearUser = createAction('clear-user')();

export const userActions = {
    getUserByEmail,
    createUser,
    deleteUser,
    getUsers,
    clearUser,
};

export type UserActions = ActionType<typeof userActions>;
