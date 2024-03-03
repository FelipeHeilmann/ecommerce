import { FastifyInstance } from "fastify"
import { TransactionRequest } from "../types/types"
import { randomUUID } from "node:crypto"

export async function createTransaction(app: FastifyInstance) {
    app.post("/payment-gateway/transactions", async (req, reply) => {
        const data = req.body as TransactionRequest

        const paymentType = data.payments[0].paymentType
        const id = randomUUID()
        let paymentUrl = null

        if (paymentType === "pix" || paymentType === "boleto") paymentUrl = "https://www.instagram.com/p/C38mWMeMSn7/?utm_source=ig_embed&ig_rid=e61afe49-0878-41c0-9b1e-b17f00f5cf76"

        console.log(paymentUrl)

        const response = {
            id,
            code: data.code,
            amount: data.items.reduce((acc, curr) => (curr.quantity * curr.amount) + acc, 0),
            currency: "BRL",
            closed: true,
            items: data.items,
            customer: {
                name: data.customer.name,
                email: data.customer.email,
                document: data.customer.document,
                type: "individual",
                address: {
                    line_1: data.customer.address.street,
                    line_2: data.customer.address.line2,
                    zip_code: data.customer.address.zipCode,
                    city: data.customer.address.city,
                    state: data.customer.address.state,
                    country: data.customer.address.country
                },
                phone: {
                    country_code: data.customer.phone.countryCode,
                    area_code: data.customer.phone.areaCode,
                    number: data.customer.phone.number
                }
            },
            payment: {
                paymentType: paymentType,
                paymentUrl: paymentUrl,
            },
            status: "created",
            created_at: new Date(),
            updated_at: new Date(),
            closed_at: new Date(),
        }

        return reply.status(200).send(response)
    })
}