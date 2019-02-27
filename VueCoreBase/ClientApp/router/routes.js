import Home from "../pages/Public/Home.vue";
import Contact from '../pages/Public/Contact.vue';
import About from '../pages/Public/About.vue';
import Login from '../pages/Public/Login.vue';
import Register from '../pages/Public/Register.vue';

const routes = [
    { path: "/", component: Home },
    { path: "/contact", component: Contact },
    { path: "/about", component: About },
    { path: "/login", component: Login },
    { path: "/register", component: Register },
    { path: "*", redirect: "/" }
];

export default routes;