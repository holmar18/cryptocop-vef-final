FROM python:3.8-slim-buster


ENV PYTHONDONTWRITEBYTECODE=1

ENV PYTHONUNBUFFERED=1


COPY /cryptocop-emails/requirements.txt /app/
WORKDIR /app
RUN pip install -r requirements.txt

WORKDIR /app
ADD /cryptocop-emails /app

RUN useradd appuser && chown -R appuser /app
USER appuser

CMD ["python", "email-service.py"]