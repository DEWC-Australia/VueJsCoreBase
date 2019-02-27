/*
 *Global constants to reduce magic numbers and strings throughout the application
 *
 * 
 * to use:
 * 
 * in the script section of a component or at the top of a js file add
 * 
 * import {constName1, constName2} from 'relative path to this file';
 * */

export const apiRoutes = {
    authenication: {
        tokenUrl: '/api/token',
        refreshUrl: '/api/token/refresh'
    },
    account: {
        registerUrl: '/api/account/register',
        confirmEmailUrl: '/api/account/confirmEmail',
        forgotEmailUrl: '/api/account/forgotEmail',
        resetPasswordUrl: '/api/account/resetPassword',
        sendVerificationEmailUrl: '/api/account/sendVerificationEmail',
        changePasswordUrl: '/api/account/changePassword',
        setPasswordUrl: '/api/account/setPassword',
        updateUrl: '/api/account/update/'
    }
};

export const httpHeaders = {
    auth: 'Authorization'
};

export const clientRoutes = {
    home: '/',
    contact: '/contact',
    about: '/about'
};

