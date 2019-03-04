<template>
    <b-jumbotron bg-variant="dark" text-variant="light" border-variant="dark">
        <b-row>

            <b-col></b-col>
            <b-col v-for="list in footerLists" :key="list.header">
                <b-row><b>{{list.header}}</b></b-row>
                <b-row v-for="link in list.links" :key="link.url">
                    <b-button @click="$router.push({ path: link.url})" size="lg" variant="link">{{link.name}}</b-button>
                </b-row>
            </b-col>
            <b-col></b-col>
        </b-row>
        <b-row>
            <div class="offset-sm-10 col-sm-2 copyright_area">
               Designed by <a href="https://colorlib.com">Colorlib.</a>
            </div>
        </b-row>
    </b-jumbotron>
</template>
<script>
    import { clientRoutes } from '../../variables/variables.js';
    export default {
        name: 'footer',
        data(){
            return{
                footerLists:
                [   
                    {
                        header: 'Tips & Help',
                        links: [
                            {
                                name:'About Us',
                                url: clientRoutes.about
                            },
                            {
                                name:'Contact Us',
                                url: clientRoutes.contact
                            }
                        ]
                    },
                    {
                        header: 'Legal and Privacy',
                        links: [
                            {
                                name:'Terms of Use',
                                url: clientRoutes.about
                            },
                            {
                                name:'Privacy Policy',
                                url: clientRoutes.about
                            },
                            {
                                name:'Privacy Policy',
                                url: clientRoutes.about
                            },
                            {
                                name:'Privacy Policy',
                                url: clientRoutes.about
                            },
                            {
                                name:'Privacy Policy',
                                url: clientRoutes.about
                            }
                        ]
                    },
                    {
                        header: 'Explore',
                        links: [
                            {
                                name:'Item 1',
                                url: clientRoutes.home
                            },
                            {
                                name:'Item 2',
                                url: clientRoutes.home
                            },
                            {
                                name:'Item 3',
                                url: clientRoutes.home
                            }
                        ]
                    }
                ]
            }
        }
    }
</script>
<style scoped>
.copyright_area {
        font: 400 14px/100px "Roboto", sans-serif;
        color: #fefefe;
        text-align: center;
    }

.copyright_area a {
            font-weight: 700;
            font-size: 14px;
            text-transform: uppercase;
            color: #f6b60b;
        }

.copyright_area a:hover, .footer_area .copyright_area a:focus {
                color: #fefefe;
            }
</style>