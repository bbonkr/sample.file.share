import React, { Suspense, useEffect } from 'react';
import { Provider } from 'react-redux';
import { BrowserRouter, Redirect, Route, Switch } from 'react-router-dom';
import { useStore } from '../../store';
import { Helmet, HelmetProvider } from 'react-helmet-async';
import { Loading } from '../Loading';
import { MessagingProvider } from '../MessagingProvider';
import { appOptions } from '../../constants';
import { EmptyRequiredAuth } from '../Empty';

import 'bulma/css/bulma.css';
import './App.css';

const Header = React.lazy(() => import('../Layouts/Header'));
const NotFound = React.lazy(() => import('../NotFound'));
const SignIn = React.lazy(() => import('../SignIn'));
const SignUp = React.lazy(() => import('../SignUp'));
const FileList = React.lazy(() => import('../FileList'));
const DownloadFile = React.lazy(() => import('../DownloadFile'));

const helmetContext = {};

export const App = () => {
    const store = useStore();

    return (
        <Provider store={store}>
            <HelmetProvider context={helmetContext}>
                <Helmet
                    title="File Sharing"
                    titleTemplate="%s - File Share Sample App"
                    defaultTitle="File Share Sample App"
                />
                <MessagingProvider>
                    <BrowserRouter>
                        <Suspense fallback={<Loading />}>
                            <Header
                                appOptions={appOptions}
                                menuRoutes={[
                                    { href: '/', title: 'My files' },
                                    { href: '/shared', title: 'Shared files' },
                                ]}
                            />
                            <Switch>
                                <Route path="/" exact>
                                    <FileList />
                                </Route>
                                <Route path="/signin" exact>
                                    <SignIn />
                                </Route>
                                <Route path="/signup" exact>
                                    <SignUp />
                                </Route>
                                <Route path="/404">
                                    <NotFound />
                                </Route>
                                <Route path="/shared">
                                    <DownloadFile />
                                </Route>
                                {/* <Route path="/chats/:id" exact>
                                        <Chat />
                                    </Route> */}
                                <Route path="/loading">
                                    <Loading />
                                </Route>
                                <Route path="*">
                                    <Redirect to="/404" />
                                </Route>
                            </Switch>
                        </Suspense>
                    </BrowserRouter>
                </MessagingProvider>
            </HelmetProvider>
        </Provider>
    );
};

export default App;
