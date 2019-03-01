<template>
    <b-alert variant="success" :show="registered && formServerErrors.length === 0">
        Registration successful. Please login to continue.
    </b-alert>
</template>
<script>
    export default {
        name: 'vee-form-success',
        props: {
            registered: {
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