<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login - RunGym</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet">
    <style>
        :root {
            --primary-color: #4361ee;
            --hover-color: #3f37c9;
            --glow-effect: 0 0 15px rgba(67, 97, 238, 0.7);
            --text-glow: 0 0 8px rgba(67, 97, 238, 0.9);
            --transition: all 0.3s ease;
        }

        body {
            font-family: 'Poppins', sans-serif;
            margin: 0;
            background-image: url('/images/Login/Fondo.jpg');
            background-size: cover;
            background-position: center;
            background-attachment: fixed;
            min-height: 100vh;
            color: #ffffff;
            display: flex;
            justify-content: center;
            align-items: center;
            backdrop-filter: blur(5px);
            padding: 2rem;
        }

        .login-container {
            max-width: 450px;
            width: 100%;
            background-color: rgba(26, 26, 27, 0.9);
            border-radius: 16px;
            box-shadow: 0 0 30px rgba(0, 0, 0, 0.5), inset 0 0 10px rgba(67, 97, 238, 0.3);
            backdrop-filter: blur(8px);
            padding: 2.5rem;
            margin: 2rem auto;
            border: 1px solid rgba(67, 97, 238, 0.2);
            animation: fadeIn 0.8s ease-out;
        }

        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        h3 {
            font-weight: 700;
            text-align: center;
            margin-bottom: 1.5rem;
            text-shadow: var(--text-glow);
            letter-spacing: 1px;
            color: white;
        }

        .form-label {
            color: #ffffff;
            font-weight: 500;
            margin-bottom: 0.5rem;
            display: block;
            transition: var(--transition);
        }

        .form-control {
            min-height: 50px;
            width: 100%;
            padding: 12px 15px;
            border-radius: 8px;
            background-color: rgba(30, 35, 60, 0.7);
            border: 1px solid rgba(67, 97, 238, 0.3);
            color: white;
            transition: var(--transition);
            box-shadow: inset 0 0 10px rgba(0, 0, 0, 0.2);
        }

        .form-control:focus {
            background-color: rgba(40, 45, 70, 0.8);
            border-color: var(--primary-color);
            box-shadow: 0 0 0 0.25rem rgba(67, 97, 238, 0.25), inset 0 0 10px rgba(67, 97, 238, 0.2);
            color: #ffffff;
            outline: none;
        }

        .form-control:hover {
            border-color: var(--primary-color);
            box-shadow: inset 0 0 15px rgba(67, 97, 238, 0.2);
        }

        .form-control::placeholder {
            color: rgba(255, 255, 255, 0.6);
        }

        .text-danger {
            color: #ff6b6b;
            font-size: 0.85rem;
            margin-top: 0.25rem;
            display: block;
        }

        .btn-primary {
            background-color: var(--primary-color);
            border: none;
            border-radius: 8px;
            padding: 12px 24px;
            font-weight: 600;
            letter-spacing: 0.5px;
            color: white;
            width: 100%;
            cursor: pointer;
            transition: all 0.4s ease;
            position: relative;
            overflow: hidden;
            box-shadow: 0 0 15px rgba(67, 97, 238, 0.5);
            margin-top: 1rem;
        }

        .btn-primary:hover {
            background-color: var(--hover-color);
            transform: translateY(-3px);
            box-shadow: 0 0 25px rgba(67, 97, 238, 0.8);
        }

        .btn-primary::before {
            content: '';
            position: absolute;
            top: -50%;
            left: -50%;
            width: 200%;
            height: 200%;
            background: linear-gradient(to bottom right, transparent, rgba(255, 255, 255, 0.2), transparent);
            transform: rotate(45deg);
            transition: all 0.6s ease;
            opacity: 0;
        }

        .btn-primary:hover::before {
            opacity: 1;
            animation: shine 1.5s ease infinite;
        }

        @keyframes shine {
            0% {
                left: -50%;
                top: -50%;
            }
            100% {
                left: 150%;
                top: 150%;
            }
        }

        .logo-container {
            margin-bottom: 2rem;
            text-align: center;
            position: relative;
        }

        .logo-container::after {
            content: '';
            position: absolute;
            bottom: -10px;
            left: 25%;
            width: 50%;
            height: 2px;
            background: linear-gradient(90deg, transparent, var(--primary-color), transparent);
        }

        .logo {
            max-width: 150px;
            height: auto;
            border-radius: 50%;
            padding: 0.5rem;
            border: 2px solid var(--primary-color);
            box-shadow: var(--glow-effect);
            transition: all 0.5s cubic-bezier(0.175, 0.885, 0.32, 1.275);
            filter: drop-shadow(0 0 8px var(--primary-color));
        }

        .logo:hover {
            transform: scale(1.1) rotate(5deg);
            box-shadow: 0 0 25px rgba(67, 97, 238, 0.9);
        }

        .input-group-text {
            background-color: rgba(30, 35, 60, 0.7);
            border: 1px solid rgba(67, 97, 238, 0.3);
            border-right: none;
            color: var(--primary-color);
            transition: var(--transition);
        }

        .input-group:hover .input-group-text {
            border-color: var(--primary-color);
            color: white;
            background-color: rgba(40, 45, 70, 0.8);
        }

        .input-group .form-control {
            border-left: none;
        }

        .input-group:hover .form-control {
            border-left: 1px solid var(--primary-color);
        }

        @media (max-width: 768px) {
            .login-container {
                padding: 1.5rem;
                margin: 1rem;
            }

            .logo {
                max-width: 120px;
            }

            h3 {
                font-size: 1.5rem;
            }
        }
    </style>
</head>
<body>
    <div class="login-container">
        <div class="logo-container">
            <img src="/images/Login/Logo.png" alt="RunGym Logo" class="logo img-fluid" aria-label="RunGym Logo">
        </div>    
        
        <h3>Iniciar sesión</h3>

        @using (Html.BeginForm("Login", "Login", FormMethod.Post))
        {
            @Html.AntiForgeryToken()

            <div class="mb-3 text-start">
                @Html.LabelFor(m => m.Correo, new { @class = "form-label" })
                <div class="input-group">
                    <span class="input-group-text"><i class="fas fa-user"></i></span>
                    @Html.TextBoxFor(m => m.Correo, new { @class = "form-control", @placeholder = "ejemplo@gmail.com" })
                </div>
                @Html.ValidationMessageFor(m => m.Correo, "", new { @class = "text-danger" })
            </div>
            <div class="mb-3 text-start">
                @Html.LabelFor(m => m.Contraseña, new { @class = "form-label" })
                <div class="input-group">
                    <span class="input-group-text"><i class="fas fa-lock"></i></span>
                    @Html.PasswordFor(m => m.Contraseña, new { @class = "form-control", @placeholder = "ej: 1234" })
                </div>
                @Html.ValidationMessageFor(m => m.Contraseña, "", new { @class = "text-danger" })
            </div>
            <button type="submit" class="btn btn-primary">Iniciar Sesión</button>
        }
    </div>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.3/jquery.validate.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.12/jquery.validate.unobtrusive.min.js"></script>
</body>
</html>