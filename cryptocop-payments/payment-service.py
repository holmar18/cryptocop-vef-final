import pika  # pip install pika
import json
from credit_card_checker import CreditCardChecker    # pip install credit-card-checker

connection = pika.BlockingConnection(pika.ConnectionParameters(host='localhost', port=5672))
channel = connection.channel()

# Sets up a queue called email-queue which is bound to the create-order routing key
name = "email-thanks"
Queue = "payment-queue"
routingKey = "create-order"

# declare exchange
a = channel.exchange_declare(exchange=name, exchange_type='direct', durable=True)

# # declare queue
b = channel.queue_declare(queue=Queue, durable=True)

# # Bind que to exchange with routing key
channel.queue_bind(queue=Queue, exchange=name, routing_key=routingKey)

print(' [*] Waiting for logs. To exit press CTRL+C')


def validateCard(data):
    print(' [*] Validating card.')
    result = CreditCardChecker(data['CreditCard']).valid()
    result = True
    print(' [%]Validation message: {0}'.format('Yes it is a valid credit card number' if result
                                               else 'No, it is not a valid credit card number'))


def validation_and_publish(ch, method, properties, data):
    JsonData = json.loads(data)
    validateCard(JsonData)
    print(' [*] Validation finished.')


channel.basic_consume(queue=Queue, on_message_callback=validation_and_publish, auto_ack=True)
channel.start_consuming()