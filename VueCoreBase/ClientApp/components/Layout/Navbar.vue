<template>
    <b-navbar toggleable="md" type="dark" variant="dark">

        <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>

        <b-navbar-brand @click="$router.push({ path: navLinks.home})" class="navbrandfix">Site Name/Logo</b-navbar-brand>

        <b-collapse is-nav id="nav_collapse">

            <b-navbar-nav>
                <!--add some <b-nav-items> if required-->

            </b-navbar-nav>

            <!-- Right aligned nav items -->
            <b-navbar-nav class="ml-auto">

                <b-nav-item @click="$router.push({ path: navLinks.login})" >Login</b-nav-item>
                <b-nav-item @click="$router.push({ path: navLinks.register})" >Register</b-nav-item>

                <b-nav-item-dropdown right>
                    <!-- Using button-content slot -->
                    <template slot="button-content">
                        <i class="far fa-user"></i>
                    </template>
                    <b-dropdown-item href="#" >Profile</b-dropdown-item>
                    <b-dropdown-item href="#">Signout</b-dropdown-item>
                </b-nav-item-dropdown>
            </b-navbar-nav>

        </b-collapse>
    </b-navbar>
</template>
<script>
    export default {
        name: "nav-bar",
        data(){
            return{
                navLinks: {
                    home: '/',
                    login: '/login',
                    register: '/register',
                    profile: '/profile'
                }
            }
        }
    }
</script>
<style>
    .navbrandfix {
        line-height: 45px;
    }

    .navitemfix {
        margin-top: 20px;
    }
</style>