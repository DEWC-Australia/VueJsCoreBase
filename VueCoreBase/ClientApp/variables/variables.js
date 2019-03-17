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
        forgotEmailUrl: '/api/account/forgotPassword',
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

export const validation = {
    passwordRelax: { required: true, max: 100 },
    passwordStrict: { required: true, min: 8, max: 100, regex: '^(?=.*[#$^+=!*()@%&])(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,100}$' },
    email: { required: true, email: true, max: 250 },
    firstName: { required: true, max: 250 },
    lastName: { required: true, max: 250 }
};


export const clientRoutes = {
    home: '/',
    contact: '/contact',
    about: '/about',
    lockout: '/lockout',
    resetPassword: '/resetpassword/:code',
    forgotLogin: '/forgotlogin',
    notAuthorised: '/notauthorised',
    setPassword: '/setpassword', // protect with admin role
    changePassword: '/changepassword'
};

