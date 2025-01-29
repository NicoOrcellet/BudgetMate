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
const addedData = [document.getElementById('addedDate'), document.getElementById('addedAmount'), document.getElementById('addedCategory')]

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
    filterForm.submit();
})

filterForm.addEventListener('submit', (event) => {
    switch (searchingMethod.value) {
        case 'date':
            if (document.getElementById('startingDate').value > document.getElementById('endingDate').value) {
                event.preventDefault()
                alert('La fecha inicial ha de ser menor o igual que la final')
            }
            break;
        case 'amount':
            if (document.getElementById('minAmount').value > document.getElementById('maxAmount').value) {
                event.preventDefault()
                alert('El monto inicial ha de ser menor o igual que el final')
            }
            break;
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

cancelAddTransaction.addEventListener('click', (event) => {
    addTransactionForm.reset();
})
