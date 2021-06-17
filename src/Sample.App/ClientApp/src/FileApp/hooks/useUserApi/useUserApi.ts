import { useSelector, useDispatch } from 'react-redux';
import { CreateUserCommand } from '../../../api';
import { PagedModelRequestBase } from '../../models/PagedModelRequestBase';
import {
    DeleteUserByIdRequest,
    GetUserByEmailRequest,
    GetUsersRequest,
} from '../../models/users';
import { rootAction } from '../../store/actions';

import { RootState } from '../../store/reducers';
import { UserState } from '../../store/reducers/user';

export const useUserApi = () => {
    const dispatch = useDispatch();

    const state = useSelector<RootState, UserState>((state) => state.user);

    return {
        ...state,
        getUserByEmailRequest: (payload: GetUserByEmailRequest) =>
            dispatch(rootAction.user.getUserByEmail.request(payload)),
        createUserRequest: (payload: CreateUserCommand) =>
            dispatch(rootAction.user.createUser.request(payload)),
        deleteUserRequest: (payload: DeleteUserByIdRequest) =>
            dispatch(rootAction.user.deleteUser.request(payload)),
        getUsersRequest: (payload: GetUsersRequest) =>
            dispatch(rootAction.user.getUsers.request(payload)),
        clearUserRequest: () => dispatch(rootAction.user.clearUser()),
        clearUsersRequest: () => dispatch(rootAction.user.clearUsers()),
        clearErrorRequest: () => dispatch(rootAction.user.clearError()),
    };
};

export type UseUserApi = ReturnType<typeof useUserApi>;
