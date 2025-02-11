const monthDisplay = document.getElementById('monthLimitDisplay')
const weekDisplay = document.getElementById('weekLimitDisplay')
const limitPeriod = document.getElementById('limitPeriod')
const addLimitButton = document.getElementById('addLimitButton')
const addLimitForm = document.getElementById('addLimitForm')
const modalCallingButtons = document.querySelectorAll('[data-bs-toggle="modal"]')
const periodValue = document.getElementById('periodValue')
const deleteLimitForm = document.querySelectorAll('#deleteLimitForm')

deleteLimitForm.forEach((form) => {
    form.addEventListener('submit', (event) => {
        if (confirm('¿Está seguro de eliminar esta transacción? Esta acción no se puede deshacer')) {
            form.submit()
        } else {
            event.preventDefault()
        }
    })
})

if (document.getElementById('savingLimit').getAttribute('data-recentPeriod') == 'week') {
    monthDisplay.style.display = 'none'
    weekDisplay.style.display = 'block'
    limitPeriod.value = "week"
} else {
    monthDisplay.style.display = 'block'
    weekDisplay.style.display = 'none'
    limitPeriod.value = "month"
}


limitPeriod.addEventListener('change', (event) => {
    if (event.target.value == 'month') {
        monthDisplay.style.display = 'block'
        weekDisplay.style.display = 'none'
    } else {
        monthDisplay.style.display = 'none'
        weekDisplay.style.display = 'block'
    }
})

modalCallingButtons.forEach(button => {
    button.addEventListener('click', () => {
        let actionType = button.getAttribute('data-actionType')
        if (actionType == 'add') {
            addLimitForm.setAttribute('action', '/Configuration/Create')
        } else {
            addLimitForm.setAttribute('action', '/Configuration/Modify')
        }
        let period = button.getAttribute('data-period')
        periodValue.value = period
        document.getElementById('limitId').value = button.getAttribute('data-limitId')
        if (period == 'Mensual') {
            let monthLimit = document.getElementById('monthLimit')
            if (monthLimit != null) {
                document.getElementById('limitAmount').value = monthLimit.textContent.replace(',', '.')
            } else {
                document.getElementById('limitAmount').value = 0
            }
        } else {
            let weekLimit = document.getElementById('weekLimit')
            if (weekLimit != null) {
                document.getElementById('limitAmount').value = weekLimit.textContent.replace(',', '.')
            } else {
                document.getElementById('limitAmount').value = 0
            }
        }
    })
})

document.querySelector('[data-bs-dismiss="modal"]').addEventListener('click', () => {
    addLimitForm.reset()
})

addLimitButton.addEventListener('click', (event) => {
    if (limitAmount.value == "") {
        event.preventDefault()
        alert('Falta ingresar el monto')
    } else {
        addLimitForm.submit()
        $('addLimitForm').modal('hide')
    }
})