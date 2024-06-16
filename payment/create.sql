create table transactions(
    id uuid primary key,
    order_id uuid,
    customer_id uuid,
    payment_service_id uuid,
    status text,
    payment_type text,
    created_at timestamp,
    approved_at timestamp,
    rejected_at timestamp
);