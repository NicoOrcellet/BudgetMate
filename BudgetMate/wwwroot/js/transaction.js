const searchingMethod = document.getElementById('searchingMethod')
const dateSelection = document.getElementById('dateSelection')
const amountSelection = document.getElementById('amountSelection')
const categorySelection = document.getElementById('categorySelection')
const clearFilters = document.getElementById('clearFilters')
const searchButton = document.getElementById('searchButton')
const filterForm = document.getElementById('filterForm')
const addTransactionButton = document.getElementById('addTransaction')
const transactionType = document.getElementById('transactionType')
const addTransactionForm = document.getElementById('transactionForm')
const cancelAddTransaction = document.getElementById('cancelAddTransaction')
const addedData = [document.getElementById('addedDate'), document.getElementById('addedAmount'), document.getElementById('CategoryId')]
const callModalButtons = document.querySelectorAll('[data-bs-toggle="modal"]')
const transactionId = document.getElementById('transactionId')
const deleteForms = document.querySelectorAll('#deleteTransactionForm')

searchingMethod.addEventListener('change', (event) => {
    switch (event.target.value) {
        case 'amount':
            dateSelection.style.display = 'none'
            categorySelection.style.display = 'none'
            amountSelection.style.display = 'block'
            break;
        case 'date':
            dateSelection.style.display = 'flex'
            categorySelection.style.display = 'none'
            amountSelection.style.display = 'none'
            break;
        case 'category':
            dateSelection.style.display = 'none'
            categorySelection.style.display = 'block'
            amountSelection.style.display = 'none'
            break;
    }
    clearFilters.style.display = 'block'
    searchButton.style.display = 'block'
})

clearFilters.addEventListener('click', (event) => {
    dateSelection.style.display = 'none'
    categorySelection.style.display = 'none'
    amountSelection.style.display = 'none'
    clearFilters.style.display = 'none'
    searchButton.style.display = 'none'
    searchingMethod.value = 'none'
    filterForm.submit()
})

function validateFilledFilterInputs(event) {
    switch (searchingMethod.value) {
        case 'date':
            if (document.getElementById('startingDate').value > document.getElementById('endingDate').value) {
                event.preventDefault()
                alert('La fecha inicial ha de ser menor o igual que la final')
                return false
            }
            return true
        case 'amount':
            if (document.getElementById('minAmount').value > document.getElementById('maxAmount').value) {
                event.preventDefault()
                alert('El monto inicial ha de ser menor o igual que el final')
                return false
            }
            return true
        default:
            return true
    }
}

filterForm.addEventListener('submit', function (event) {
    if (validateFilledFilterInputs(event)) {
        filterForm.submit()
    } else {
        event.preventDefault()
    }
})

function areRequiredInputsFilled() {
    var validation = true
    addedData.forEach(input => {
        if (input.value == "") {
            validation = false
        }
    })
    return validation
}

addTransactionButton.addEventListener('click', (event) => {
    if (areRequiredInputsFilled()) {
        addTransactionForm.submit()
        bootstrap.Modal.getInstance(document.getElementById('creationModal')).hide()
    } else {
        event.preventDefault()
        alert('Falta información necesaria')
    }
})

callModalButtons.forEach(button => {
    button.addEventListener('click', function () {
        const op = this.getAttribute('data-operationType')
        if (op == 'Modify') {
            const row = this.closest('tr')
            const values = Array.from(row.querySelectorAll('td:not(:last-child)')).map(td => td.textContent)
            document.getElementById('addedAmount').value = parseFloat(values[0].replace(',','.'))
            const parts = values[1].split('/')
            document.getElementById('addedDate').value = `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}`;
            document.getElementById('TransactionDescription').value = values[2]
            const CategoryId = document.getElementById('CategoryId')
            CategoryId.value = Array.from(CategoryId.options).find(option => option.textContent == values[3]).value
            addTransactionForm.setAttribute('action', '/MoneyTransaction/Modify')
            addTransactionButton.textContent = 'Modificar'
            transactionId.value = row.getAttribute('data-transactionId')
            transactionType.value = row.getAttribute('data-transactionType')
        } else {
            addTransactionForm.reset()
            addTransactionForm.setAttribute('action','/MoneyTransaction/Create')
            addTransactionButton.textContent = 'Agregar'
        }
    })
})

deleteForms.forEach((form) => {
    form.addEventListener('submit', (event) => {
        if (confirm('¿Está seguro de eliminar esta transacción? Esta acción no se puede deshacer')) {
            form.submit()
        } else {
            event.preventDefault()
        }
    })
})