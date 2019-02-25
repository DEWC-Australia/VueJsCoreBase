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
    </b-jumbotron>
</template>
<script>
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
                                url: '/about'
                            },
                            {
                                name:'Contact Us',
                                url: '/contact'
                            }
                        ]
                    },
                    {
                        header: 'Legal and Privacy',
                        links: [
                            {
                                name:'Terms of Use',
                                url: '/about'
                            },
                            {
                                name:'Privacy Policy',
                                url: '/about'
                            },
                            {
                                name:'Posting Policy',
                                url: '/about'
                            },
                            {
                                name:'Transport Hazard Classifications',
                                url: '/about'
                            },
                            {
                                name:'Parcel Types',
                                url: '/about'
                            }
                        ]
                    },
                    {
                        header: 'Explore',
                        links: [
                            {
                                name:'Item 1',
                                url: '/'
                            },
                            {
                                name:'Item 2',
                                url: '/'
                            },
                            {
                                name:'Item 3',
                                url: '/'
                            }
                        ]
                    }
                ]
            }
        }
    }
</script>