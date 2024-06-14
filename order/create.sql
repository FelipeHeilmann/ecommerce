create table customers(
    id uuid primary key,
    name text,
    email text,
    cpf text,
    phone text,
    password text,
    birth_date timestamp,
    created_at timestamp
);

create table categories(
	id uuid primary key,
	name text,
	description text
);

create table addresses(
	id uuid primary key,
	customer_id uuid,
	zip_code text,
	street text,
	neighborhood text, 
	complement text,
	number text,
	city text,
	state text,
	country text,
	
	foreign key(customer_id) references customers(id)
);

create table coupons(
    id uuid primary key,
    name text,
    value numeric,
    expiress_at timestamp
);

create table products(
	id uuid primary key,
	name text,
	description text,
	image_url text,
	currency text,
	amount numeric,
	sku text,
	created_at timestamp,
	category_id uuid
);

create table orders(
	id uuid primary key,
	customer_id uuid,
	coupon_id uuid,
	status text,
	billing_address_id uuid,
	shipping_address_id uuid,
	created_at timestamp,
	updated_at timestamp,

	foreign key(customer_id) references customers(id),
	foreign key(coupon_id) references coupons(id),
	foreign key(billing_address_id) references addresses(id),
	foreign key(shipping_address_id) references addresses(id)
);

create table line_item(
	id uuid primary key,
	order_id uuid,
	product_id uuid,
	currency text,
	amount numeric,
	quantity numeric,

	foreign key(order_id) references orders(id),
	foreign key(product_id) references products(id)
);
