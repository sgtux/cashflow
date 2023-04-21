import { toast } from 'react-toastify'

const config = {
    hideProgressBar: true,
    pauseOnHover: true,
}

export default {
    error: message => toast.error(message, config),
    success: message => toast.success(message, config),
    warning: message => toast.warning(message, config),
    info: message => toast.info(message, { hideProgressBar: true, autoClose: 2000 }),
}