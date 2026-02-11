<style src="../assets/home.css"></style>

<template>
  <div class="home">
    <Toast ref="toastRef" />

    

    <!-- HEADER -->
    <header class="header">
      <h2>Dobrodošli na AutoPlac - {{ username }}</h2>
      <button class="logout" @click="logout">Logout</button>
    </header>

    <!-- SVA VOZILA -->
    <section>
      <h3>Sva vozila</h3>

      <div class="grid">
        <div v-for="v in vehicles" :key="v.id" class="card">
          <h4>{{ v.brand }} {{ v.model }}</h4>
          <p>Godina: {{ v.year }}</p>
          <p>Cena: {{ v.price }} €</p>
          <p>{{ v.description }}</p>

          <!-- USER -->
          <button class="purchase-btn" v-if="role === 'Buyer'" @click="sendOrder(v.id)">
            Pošalji porudžbinu
          </button>

          <!-- ADMIN -->
          <button v-if="role === 'Admin'" class="danger" @click="deleteVehicleAdmin(v.id)">
            Obriši
          </button>
        </div>
      </div>
    </section>



    <!-- ADMIN – KORISNICI -->
    <section v-if="role === 'Admin'" class="admin-users">
      <h3>Svi korisnici</h3>

      <table class="users-table">
        <thead>
          <tr>
            <th>Username</th>
            <th>Email</th>
            <th>Rola</th>
            <th>Akcije</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="u in users" :key="u.id">
            <td>{{ u.username }}</td>
            <td>{{ u.email }}</td>
            <td>{{ u.role }}</td>
            <td>
              <button class="danger" @click="deleteUser(u.id)">
                Obriši
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- MOJA VOZILA (SAMO OWNER) -->
    <section v-if="role === 'Owner'" class="my-section">
      <h3>Moja vozila</h3>

      <div class="add-vehicle">
        <button class="add-btn" @click="showAddForm = true">
          + Dodaj vozilo
        </button>
      </div>
      <div v-if="showAddForm" class="edit-card">
        <h3>Dodaj novo vozilo</h3>

        <input v-model="newVehicle.brand" placeholder="Brend" />
        <input v-model="newVehicle.model" placeholder="Model" />
        <input type="number" v-model="newVehicle.year" placeholder="Godina" />
        <input type="number" v-model="newVehicle.price" placeholder="Cena" />
        <textarea v-model="newVehicle.description" placeholder="Opis"></textarea>

        <div class="actions">
          <button class = "save" @click="createVehicle">Sačuvaj</button>
          <button class="cancel" @click="showAddForm = false">Otkaži</button>
        </div>
      </div>

      <div v-if="myVehicles.length === 0">
        Nemate svoja vozila.
      </div>

      <div class="grid">
        <div v-for="v in myVehicles" :key="v.id" class="card my-card">
          <h4>{{ v.brand }} {{ v.model }}</h4>
          <p>Godina: {{ v.year }}</p>
          <p>Cena: {{ v.price }} €</p>

          <div class="actions">
            <button class = "izmeni" @click="editVehicle(v)">
              Izmeni
            </button>

            <button class="danger" @click="deleteVehicle(v.id)">
              Izbriši
            </button>
          </div>
          
        </div>
      </div>
    </section>

    <!-- MOJE PORUDZBINA (SAMO KUPAC) -->

    <section v-if="role === 'Buyer'" class="table-wrapper">
  <h3>Moje porudžbine</h3>

  <div v-if="myPurchases.length === 0" class="empty">
    Nemate porudžbine.
  </div>

  <table v-else class="table">
    <thead>
      <tr>
        <th>Vozilo</th>
        <th>Prodavac</th> 
        <th>Datum</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="p in myPurchases" :key="p.id">
        <td>{{ p.vehicleName }}</td>
        <td>{{ p.ownerUsername }}</td> 
        <td>{{ new Date(p.createdAt).toLocaleString() }}</td>
      </tr>
    </tbody>
  </table>
</section>

    <!-- MOJE PORUDZBINE (SAMO OWNER) -->

    <section v-if="role === 'Owner'" class="table-wrapper">
  <h3>Porudžbine za moja vozila</h3>

  <div v-if="ownerPurchases.length === 0" class="empty">
    Trenutno nema porudžbina.
  </div>
    
  <table v-else class="table">
    <thead>
      <tr>
        <th>Vozilo</th>
        <th>Kupac</th>
        <th>Email</th>
        <th>Datum</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="p in ownerPurchases" :key="p.id">
        <td>{{ p.vehicleName }}</td>
        <td>{{ p.buyerUsername }}</td>
        <td>{{ p.buyerEmail }}</td>
        <td>{{ new Date(p.createdAt).toLocaleString() }}</td>
      </tr>
    </tbody>
  </table>
</section>

    <!-- EDIT VOZILA -->
<div v-if="editingVehicle" class="edit-card">
  <h3>Izmena vozila</h3>

  <input v-model="editingVehicle.brand" placeholder="Brend" />
  <input v-model="editingVehicle.model" placeholder="Model" />
  <input type="number" v-model="editingVehicle.year" placeholder="Godina" />
  <input type="number" v-model="editingVehicle.price" placeholder="Cena" />
  <textarea v-model="editingVehicle.description" placeholder="Opis"></textarea>

  <div class="actions">
    <button @click="updateVehicle" class="izmeni" >Sačuvaj</button>
    <button class="cancel" @click="editingVehicle = null">Otkaži</button>
  </div>
</div>

  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axios'
import Toast from '../components/Toast.vue'

const router = useRouter()

const vehicles = ref([])
const editingVehicle = ref(null)
const toastRef = ref(null)

const role = localStorage.getItem('role')
const userId = Number(localStorage.getItem('userId'))
const username = localStorage.getItem('username')
const users = ref([])
const myPurchases = ref([])
const ownerPurchases = ref([])

console.log('TOKEN:', localStorage.getItem('token'))

const logout = () => {
  localStorage.clear()
  router.push('/login')
}

const loadVehicles = async () => {
  const res = await api.get('/vehicles')
  vehicles.value = res.data
}

const loadMyPurchases = async () => {
  try {
    const res = await api.get('/purchases/my')
    myPurchases.value = res.data
  } catch (err) {
    console.error(err)
  }
}

const loadOwnerPurchases = async () => {
  try {
    const res = await api.get('/purchases/for-my-vehicles')
    ownerPurchases.value = res.data
  } catch (err) {
    console.error(err)
  }
}

const loadUsers = async () => {
  if (role !== 'Admin') return

  try {
    const res = await api.get('/users')
    users.value = res.data
  } catch (err) {
    toastRef.value.showToast('Greška pri učitavanju korisnika')
  }
}

const myVehicles = computed(() =>
  vehicles.value.filter(v => v.ownerId === userId)
)

const editVehicle = (vehicle) => {
  console.log('EDIT CLICKED', vehicle)
  editingVehicle.value = { ...vehicle }
}

const updateVehicle = async () => {
  try {
    const res = await api.put(
      `/vehicles/${editingVehicle.value.id}`,
      editingVehicle.value
    )

    //  NADJI INDEX U LISTI
    const index = vehicles.value.findIndex(
      v => v.id === editingVehicle.value.id
    )

    //  ZAMENI VOZILO U MEMORIJI
    if (index !== -1) {
      vehicles.value[index] = { ...editingVehicle.value }
    }
    
    toastRef.value.showToast('Vozilo uspešno izmenjeno')
    

    editingVehicle.value = null
    await loadOwnerPurchases()
  } catch (err) {
    toastRef.value.showToast('Greška pri izmeni')
  }
}

const deleteVehicle = async (id) => {
  const confirmed = confirm(
    'Da li ste sigurni da želite da obrišete svoj oglas?'
  )

  if (!confirmed) return

  try {
    await api.delete(`/vehicles/${id}`)

    // ukloni iz memorije (nema refresha)
    vehicles.value = vehicles.value.filter(v => v.id !== id)

    await loadOwnerPurchases()

    toastRef.value.showToast('Oglas je uspešno obrisan')
  } catch (err) {
    toastRef.value.showToast('Greška pri brisanju')
  }
}

const deleteVehicleAdmin = async (id) => {
  const confirmed = confirm(
    'Da li ste sigurni da želite da obrišete ovaj oglas?'
  )

  if (!confirmed) return

  try {
    await api.delete(`/vehicles/${id}`)

    //  odmah ukloni iz UI
    vehicles.value = vehicles.value.filter(v => v.id !== id)

    toastRef.value.show('Oglas je obrisan (ADMIN) ', 'success')
  } catch (err) {
    toastRef.value.show('Greška pri brisanju ', 'error')
  }
}

const createVehicle = async () => {
  try {
    await api.post('/vehicles', newVehicle.value)

    await loadVehicles() // await sluzi da frontend saceka da backend load-uje i vrati mu nazad kako bi odma prikazao

    showAddForm.value = false

    newVehicle.value = {
      brand: '',
      model: '',
      year: '',
      price: '',
      description: ''
    }

    toastRef.value.showToast('Vozilo je uspešno dodato')
  } catch (err) {
    console.error(err)
    toastRef.value.showToast('Greška pri dodavanju vozila')
  }
}

const sendOrder = async (vehicleId) => {
  try {
    await api.post('/purchases', { vehicleId })

    toastRef.value.showToast(
      'Uspešno ste poslali porudžbinu. Prodavac će vas kontaktirati u najkraćem roku.'
    )
    await loadMyPurchases()
  } catch (err) {
    toastRef.value.showToast(
      err.response?.data || 'Greška pri slanju porudžbine'
    )
  }
}

const deleteUser = async (userId) => {
  const confirmed = confirm(
    'Da li ste sigurni da želite da obrišete korisnika?\nAko je owner, biće obrisani i njegovi oglasi.'
  )

  if (!confirmed) return

  try {
    await api.delete(`/users/${userId}`)
    await loadUsers()
    vehicles.value = vehicles.value.filter(v => v.ownerId !== userId)
    toastRef.value.showToast('Korisnik je obrisan')
    
  } catch (err) {
    toastRef.value.showToast(
      err.response?.data || 'Greška pri brisanju korisnika'
    )
  }
}

const showAddForm = ref(false)

const newVehicle = ref({
  brand: '',
  model: '',
  year: '',
  price: '',
  description: ''
})

onMounted(() => {
  loadVehicles()
  loadUsers()

  if (role === 'Buyer') {
    loadMyPurchases()
  }

  if (role === 'Owner') {
  loadOwnerPurchases()
  }
})


</script>
