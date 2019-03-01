<template>
    <b-alert variant="danger" :show="formServerErrors.length !== 0" dismissible
             @dismissed="formServerErrors = null">
        <div v-for="(error, index) in formServerErrors" :key="index">
            {{ error }}
        </div>
    </b-alert>
</template>
<script>
    export default {
        name: 'vee-form-server-errors',
        props: {
            formServerErrors: {
                type: Array,
                required: true
            }
        }

    }
</script>