// import the components to route to
import Home from "../pages/Public/Home.vue";
import Contact from '../pages/Public/Contact.vue';
import About from '../pages/Public/About.vue';

// public user account routes
import UserLockout from '../pages/Public/Lockout.vue';
import ForgotLogin from '../pages/Public/ForgotLogin.vue';
import ResetPassword from '../pages/Public/ResetPassword.vue';
import NotAuthorised from '../pages/Public/NotAuthorised.vue';

// Admin routes
import SetPassword from '../pages/Areas/Account/SetPassword.vue';

// User routes
import ChangePassword from '../pages/Areas/Account/ChangePassword.vue';

import { clientRoutes } from '../variables/variables.js';

// Applications client-side route definitions
const routes = [
    { path: clientRoutes.home, component: Home },
    { path: clientRoutes.contact, component: Contact },
    { path: clientRoutes.about, component: About },
    { path: clientRoutes.lockout, component: UserLockout },
    { path: clientRoutes.resetPassword, component: ResetPassword },
    { path: clientRoutes.forgotLogin, component: ForgotLogin },
    { path: clientRoutes.notAuthorised, component: NotAuthorised },
    {
        path: clientRoutes.setPassword,
        component: SetPassword,
        meta: { requiresAuth: true, role: "Admin", redirect: clientRoutes.notAuthorised }
        
    },
    {
        path: clientRoutes.changePassword,
        component: ChangePassword,
        meta: { requiresAuth: true, role: "User", redirect: clientRoutes.notAuthorised }
    },

    { path: "*", redirect: clientRoutes.home }
];

export default routes;