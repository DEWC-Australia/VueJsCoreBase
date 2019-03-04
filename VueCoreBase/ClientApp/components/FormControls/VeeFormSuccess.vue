<template>
    <b-alert variant="success" :show="formSubmitted && formServerErrors.length === 0">
        {{message}}
    </b-alert>
</template>
<script>
    export default {
        name: 'vee-form-success',
        props: {
            formSubmitted: {
                type: Boolean,
                required: true
            },
            formServerErrors: {
                type: Array,
                required: true
            },
            message: {
                type: String,
                required: true
            }
        }
    }
</script>