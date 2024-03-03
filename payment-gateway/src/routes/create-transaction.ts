import { FastifyInstance } from "fastify"
import { TransactionRequest } from "../types/types"
import { randomUUID } from "node:crypto"

export async function createTransaction(app: FastifyInstance) {
    app.post("/payment-gateway/transactions", async (req, reply) => {
        const data = req.body as TransactionRequest

        const id = randomUUID()
        let paymentUrl = null
        if (data.payments.PaymentType === "Pix" || data.payments) paymentUrl = "httpshttps://www.instagram.com/p/C38mWMeMSn7/?utm_source=ig_embed&ig_rid=e61afe49-0878-41c0-9b1e-b17f00f5cf76"

        console.log(id)

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
                phones: {
                    country_code: data.customer.phone.countryCode,
                    area_code: data.customer.phone.areaCode,
                    number: data.customer.phone.number
                }
            },
            payment: {
                payment_type: data.payments.PaymentType,
                payment_url: paymentUrl,
            },
            status: "created",
            created_at: new Date().toISOString(),
            updated_at: new Date().toISOString(),
            closed_at: new Date().toISOString(),
        }

        return reply.status(200).send(response)
    })
}