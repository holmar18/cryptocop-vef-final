import pika
import json
import requests


def email(data):
    return requests.post(
        "https://api.mailgun.net/v3/sandboxbb86179bf55d46f4998e0eb25774e584.mailgun.org/messages",
        auth=("api", "fd59924fb28750fb5623b0684c81044b-ea44b6dc-b056e537"),
        data={"from": "Hólmar Karl <mailgun@sandboxbb86179bf55d46f4998e0eb25774e584.mailgun.org>",
              "to": ["{0}".format(data['Email']), "{0}".format(data['Email'])],
              "subject": "Recipt for order number: %d" % data['Id'],
              "text": """Thank you for your purchase\n\n
                        Name: {0}\n\n
                        Address: {1}\t City: {2}\tzip code: {3}\n
                        Country: {4}\tDate: {5}\n
                        Total: {6}\n\n
                        Order: {7}""".format(data['FullName']
                                       ,data['StreetName'] + ", " + data['HouseNumber'],data['City'], data['ZipCode']
                                       ,data['Country'], data['OrderDate'], 
                                        data['TotalPrice'],
                                        data['OrderItems'])}
    )


connection = pika.BlockingConnection(pika.ConnectionParameters(host='localhost', port=5672))
channel = connection.channel()

# Sets up a queue called email-queue which is bound to the create-order routing key
name = "email-thanks"
Queue = "email-queue"
routingKey = "create-order"

# declare exchange
channel.exchange_declare(exchange=name, exchange_type='direct', durable=True)

# declare queue
channel.queue_declare(queue=Queue, durable=True)

# Bind que to exchange with routing key
channel.queue_bind(queue=Queue, exchange=name, routing_key=routingKey)

print(' [*] Waiting for logs. To exit press CTRL+C')


def email_and_publish(ch, method, properties, data):
    JsonData = json.loads(data);
    email(JsonData)
    print(' [*] Email sent.')


channel.basic_consume(queue=Queue, on_message_callback=email_and_publish, auto_ack=True)
channel.start_consuming()
