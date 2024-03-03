import fastify from "fastify"
import { createTransaction } from "./routes/create-transaction"


const app = fastify()

app.register(createTransaction)

app.listen({
    host: '0.0.0.0',
    port: 3333
}).then(() => console.log("Server running"))