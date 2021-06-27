import { toast } from 'react-toastify'

const config = {
    hideProgressBar: true, pauseOnHover: true
}

export default {
    error: message => toast.error(message, config),
    success: message => toast.success(message, config),
}