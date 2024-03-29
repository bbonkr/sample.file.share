import React, { useEffect, useState } from 'react';
import { LinkModel, AppOptions } from '../../models';
import { GenericLink } from '../GenericLink';
import { FaGithub, FaBookOpen } from 'react-icons/fa';
import { useUserApi } from '../../hooks/useUserApi';

import './Header.css';

interface HeaderProps {
    appOptions?: AppOptions;
    menuRoutes?: LinkModel[];
}

export const Header = ({ appOptions, menuRoutes }: HeaderProps) => {
    const { user, clearUserRequest } = useUserApi();

    const [navbarMenuIsActive, setNavbarMenuIsActive] = useState(false);
    const handleClickMenu = () => {
        window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
        setNavbarMenuIsActive((prevState) => false);
    };

    const handleToggleMenu = (
        event: React.MouseEvent<HTMLAnchorElement, MouseEvent>,
    ) => {
        event.preventDefault();
        setNavbarMenuIsActive((prevState) => !prevState);
    };

    const handleClickOuter = () => {
        setNavbarMenuIsActive((_) => false);
    };

    const handleClickSignout = () => {
        clearUserRequest();
    };

    const handleWindowResize = () => {
        const headerNavElement =
            document.querySelector<HTMLDivElement>('#header-nav');
        if (headerNavElement) {
            const styles = window.getComputedStyle(headerNavElement);
            const displayValue = styles.getPropertyValue('display');

            if (displayValue === 'flex') {
                setNavbarMenuIsActive((_) => false);
            }
        }
    };

    const handleClickBack = () => {};

    useEffect(() => {
        // let observer: MutationObserver;

        // const headerNavElement = document.querySelector<HTMLDivElement>('#header-nav');
        // if (headerNavElement) {
        //     const config: MutationObserverInit = {
        //         attributes: true,
        //         childList: true,
        //         characterData: true,
        //     };
        //     const callback = (mutations: MutationRecord[], observer: MutationObserver) => {
        //         console.info(observer);
        //         for (const mutation of mutations) {
        //             if (mutation.type === 'childList') {
        //                 console.info('observer childList');
        //             }
        //             if (mutation.type === 'attributes') {
        //                 console.info('observer attributeName', mutation.attributeName);
        //             }
        //         }
        //     };
        //     observer = new MutationObserver(callback);
        //     observer.observe(headerNavElement, config);
        // }

        window.addEventListener('resize', handleWindowResize);
        handleWindowResize();

        return () => {
            // if (observer) {
            //     observer.disconnect();
            // }
            window.removeEventListener('resize', handleWindowResize);
        };
    }, []);

    if (!user) {
        return <React.Fragment></React.Fragment>;
    }

    return (
        <>
            {navbarMenuIsActive && (
                <div
                    style={{
                        position: 'fixed',
                        top: 0,
                        left: 0,
                        right: 0,
                        bottom: 0,
                        background: '#000000',
                        opacity: 0.7,
                        zIndex: 1000,
                    }}
                    onClick={handleClickOuter}
                ></div>
            )}
            <nav
                className="navbar is-fixed-top has-dropdown is-dark height-fix-56"
                id="header-nav"
                role="navigation"
                aria-label="main navigation"
                style={{ zIndex: 1001 }}
            >
                <div className="navbar-brand">
                    <GenericLink
                        record={{ href: '/', title: '' }}
                        classNames={['navbar-item']}
                        onClick={handleClickMenu}
                    >
                        <span className="ml-3">{appOptions?.title}</span>
                    </GenericLink>
                    <a
                        role="button"
                        className={`navbar-burger ${
                            navbarMenuIsActive ? 'is-active' : ''
                        }`}
                        aria-label="menu"
                        aria-expanded="false"
                        data-target="navbarMainHeader"
                        onClick={handleToggleMenu}
                    >
                        <span aria-hidden="true"></span>
                        <span aria-hidden="true"></span>
                        <span aria-hidden="true"></span>
                    </a>
                </div>

                <div
                    id="navbarMainHeader"
                    className={`navbar-menu ${
                        navbarMenuIsActive ? 'is-active' : ''
                    }`}
                    onBlurCapture={() => setNavbarMenuIsActive((_) => false)}
                >
                    <div className="navbar-start">
                        {menuRoutes?.map((menu) => {
                            return (
                                <GenericLink
                                    classNames={['navbar-item']}
                                    record={{
                                        href: menu.href,
                                        title: menu.title,
                                    }}
                                    key={menu.href}
                                    onClick={handleClickMenu}
                                />
                            );
                        })}
                    </div>

                    <div className="navbar-end">
                        <div className="navbar-item">
                            <div className="buttons">
                                {appOptions && appOptions?.github && (
                                    <a
                                        className="button"
                                        href={appOptions.github}
                                        target="_blank"
                                        rel="external"
                                        title="Navigate to github repository page"
                                    >
                                        <FaGithub />
                                    </a>
                                )}
                                <a
                                    className="button"
                                    href="/swagger"
                                    target="_blank"
                                    rel="external"
                                    title="Navigate to api specification page"
                                >
                                    <FaBookOpen />
                                </a>
                            </div>
                        </div>

                        <div className="navbar-item">
                            <button
                                className="button "
                                onClick={handleClickSignout}
                            >
                                Sign out
                            </button>
                        </div>
                    </div>
                </div>
            </nav>
        </>
    );
};

export default Header;
