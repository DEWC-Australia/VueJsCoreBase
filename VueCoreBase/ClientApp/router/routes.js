// import the components to route to
import Home from "../pages/Public/Home.vue";
import Contact from '../pages/Public/Contact.vue';
import About from '../pages/Public/About.vue';
import { clientRoutes } from '../variables/variables.js';

// Applications client-side route definitions
const routes = [
    { path: clientRoutes.home, component: Home },
    { path: clientRoutes.contact, component: Contact },
    { path: clientRoutes.about, component: About },
    { path: "*", redirect: "/" }
];

export default routes;