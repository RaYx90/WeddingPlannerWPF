-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost
-- Tiempo de generación: 16-06-2019 a las 16:31:35
-- Versión del servidor: 8.0.13-4
-- Versión de PHP: 7.2.19-0ubuntu0.18.04.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `y6CQ6X1U7Z`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `clientes`
--

CREATE TABLE `clientes` (
  `id` int(11) NOT NULL,
  `dni` varchar(9) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `password` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `apellidos` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `direccion` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `poblacion` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `provincia` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `cp` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `fnac` int(11) DEFAULT '1559979585',
  `edad` int(11) DEFAULT '0',
  `telefono` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `movil` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `clientes`
--

INSERT INTO `clientes` (`id`, `dni`, `email`, `password`, `nombre`, `apellidos`, `direccion`, `poblacion`, `provincia`, `cp`, `fnac`, `edad`, `telefono`, `movil`) VALUES
(1, '44556677P', 'cliente@prueba.info', 'b11c5a1de84419e25698802d58f8dc27', 'Juan', 'Perez Diaz', 'Calle Aspe', 'Alicante', 'Alicante', '03012', 1559979585, 30, '666000666', '666666666'),
(2, '55667788L', 'cliente2@prueba.info', 'b11c5a1de84419e25698802d58f8dc27', 'Ana', 'Carmena Colmenar', 'Calle Sevilla', 'Alicante', 'Alicante', '03012', 1559979585, 35, '666111666', '666666666'),
(3, '55669988Q', 'cliente3@prueba.info', 'b11c5a1de84419e25698802d58f8dc27', 'Roberto', 'Sanpere Mingó', 'Calle San Vicente', 'Alicante', 'Alicante', '03010', 1559979585, 30, '666000666', '666202666'),
(4, '77558899J', 'cliente4@prueba.info', 'b11c5a1de84419e25698802d58f8dc27', 'Luisa', 'Castellar Longo', 'Calle San Dolores', 'Elche', 'Elche', '03625', 1559979585, 25, '777666999', '777888555');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `eventos`
--

CREATE TABLE `eventos` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
  `fecha` int(11) DEFAULT '1559979585',
  `activo` int(1) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `eventos`
--

INSERT INTO `eventos` (`id`, `nombre`, `descripcion`, `fecha`, `activo`) VALUES
(1, 'Boda de Juan y Ana', 'Nos alegra haber dado este paso tan importante en nuestras vidas, ahora nos toca ir preparando todo para este evento tan esperado.\r\n\r\nGracias a todos los que participeís.', 1559979804, 1),
(2, 'Boda de Roberto y Luisa', 'Por fin llegó el día, en el que nos casamos, esperamos que todo salga bien para así disfrutar al máximo de ese día.', 1561448604, 1),
(3, 'Boda Carlos y Ricarda', 'Boda de Carlos', 1561680000, 1),
(4, 'Boda de Luis y Carmen', 'Boda de Luis y Carmen', 1561248000, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `mensajes`
--

CREATE TABLE `mensajes` (
  `id` int(11) NOT NULL,
  `ref_evento` int(11) NOT NULL,
  `ref_proveedor` int(11) DEFAULT NULL,
  `ref_cliente` int(11) DEFAULT NULL,
  `mensaje` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `fecha` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `mensajes`
--

INSERT INTO `mensajes` (`id`, `ref_evento`, `ref_proveedor`, `ref_cliente`, `mensaje`, `fecha`) VALUES
(1, 1, NULL, 1, 'Hola, ¿nos puede hacer un presupuesto de la fotografia de preboda, boda y postboda?\r\n\r\nGracias.', 1559980103),
(2, 1, 1, NULL, 'Su proveedor Proveedor de prueba le acaba de generar un presupuesto, se lo enviará por email a la mayor brevedad posible.', 1559993699),
(3, 1, 1, NULL, 'Su proveedor Proveedor de prueba le acaba de actualizar el presupuesto número 20190001, se lo enviará por email a la mayor brevedad posible con los cambios.', 1559993880),
(4, 1, NULL, 2, 'Hola,\r\n\r\n¿Nos puede aplicar el IVA reducido?\r\n\r\nGracias.', 1559989666),
(5, 1, 1, NULL, 'De acuerdo, lo reviso y envio nuevamente.', 1559997610),
(7, 1, 1, NULL, 'Buenos días,   He comprobado que me han asociado a su evento, cualquier duda o pregunta no duden en indicarmelo.  Saludos.', 1560000711),
(8, 4, 1, NULL, 'Buenos días,   He comprobado que me han asociado a su evento, cualquier duda o pregunta no duden en indicarmelo.  Saludos.', 1560000751),
(13, 1, NULL, NULL, 'Su proveedor Proveedor de prueba ha eliminado el presupuesto 20190001. Si cree que esto no es correcto, escríbale un mensaje.', 1560001712),
(14, 1, NULL, 1, 'Rehaganoslo sin la postboda.\r\n\r\nSentimos los inconvenientes.', 1560001713),
(15, 1, NULL, NULL, 'Su proveedor Proveedor de prueba le acaba de generar un presupuesto, se lo enviará por email a la mayor brevedad posible.', 1560001872),
(16, 1, 1, NULL, 'HOla,  Por favor, indicarme si este ya esta bien para marcarlo como Aceptado.  Un saludo.,', 1560001924),
(17, 1, NULL, 2, 'Si, ya esta.\r\n\r\nGracias por todo, hablamos pronto.', 1560001925),
(18, 1, 1, NULL, 'Su proveedor Proveedor de prueba le acaba de actualizar el presupuesto número 20190001, se lo enviará por email a la mayor brevedad posible con los cambios.', 1560002036),
(19, 1, 1, NULL, 'Su proveedor Proveedor de prueba le acaba de actualizar el presupuesto número 20190001 y se lo ha marcado como aceptado, se lo enviará por email a la mayor brevedad posible con los cambios.', 1560002036),
(20, 3, NULL, 4, 'Hola, \r\n\r\nUna boda de 100 invitados, 70 adultos y 30 niños ¿por cuanto sale aproximadamente?\r\n\r\nGracias.', 1560002038),
(21, 3, NULL, NULL, 'Su proveedor Proveedor de prueba le acaba de generar un presupuesto, se lo enviará por email a la mayor brevedad posible.', 1560002261),
(22, 1, 1, NULL, 'Hola Carlos,  Ya le he enviado el presupuesto con el coste aproximado + IVA, recordarle que esto es muy orientativo pero dentro del precio esta incluida la barra libre.  Saludos.', 1560002316),
(23, 3, NULL, 3, 'Muchisimas gracias, le decimos algo pronto.', 1560002319),
(24, 4, 1, NULL, 'Hola Luis y Carmen,  Tal y como acabamos de hablar por teléfono os voy a enviar el presupuesto ya mismo para que lo poadís valorar el fin de semana.  Gracias.', 1560002513),
(25, 2, NULL, NULL, 'Su proveedor Proveedor de prueba le acaba de generar un presupuesto, se lo enviará por email a la mayor brevedad posible.', 1560002603),
(26, 2, 1, NULL, 'Disculpeme Luisa, ese presupuesto no era para usted, obvie este mensaje.,  Saludos.', 1560002656),
(27, 2, NULL, NULL, 'Su proveedor Proveedor de prueba ha eliminado el presupuesto 20190003. Si cree que esto no es correcto, escríbale un mensaje.', 1560002667),
(28, 4, NULL, NULL, 'Su proveedor Proveedor de prueba le acaba de generar un presupuesto, se lo enviará por email a la mayor brevedad posible.', 1560002827);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `presupuestos`
--

CREATE TABLE `presupuestos` (
  `id` int(11) NOT NULL,
  `numero` int(11) DEFAULT NULL,
  `importe_bruto` decimal(10,2) DEFAULT NULL,
  `tipo_iva` int(1) DEFAULT NULL,
  `importe_iva` decimal(10,2) DEFAULT NULL,
  `importa_neto` decimal(10,2) DEFAULT NULL,
  `ref_cliente` int(11) DEFAULT NULL,
  `ref_proveedor` int(11) DEFAULT NULL,
  `fecha` date DEFAULT NULL,
  `aceptado` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `presupuestos`
--

INSERT INTO `presupuestos` (`id`, `numero`, `importe_bruto`, `tipo_iva`, `importe_iva`, `importa_neto`, `ref_cliente`, `ref_proveedor`, `fecha`, `aceptado`) VALUES
(1, 20190001, '750.00', 10, '75.00', '825.00', 2, 1, '2019-06-08', 1),
(2, 20190002, '8500.00', 21, '3255.00', '11755.00', 1, 1, '2019-06-08', 0),
(3, 20190003, '4705.00', 21, '3070.20', '7775.20', 3, 1, '2019-06-08', 1),
(4, 20190004, '635.00', 10, '63.50', '698.50', 4, 1, '2019-06-16', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `presupuestos_lineas`
--

CREATE TABLE `presupuestos_lineas` (
  `id` int(11) NOT NULL,
  `ref_presupuesto` int(11) NOT NULL,
  `descripcion` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `cantidad` decimal(10,2) DEFAULT NULL,
  `importe_bruto` decimal(10,2) DEFAULT NULL,
  `importe_iva` decimal(10,2) DEFAULT NULL,
  `importe_neto` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `presupuestos_lineas`
--

INSERT INTO `presupuestos_lineas` (`id`, `ref_presupuesto`, `descripcion`, `cantidad`, `importe_bruto`, `importe_iva`, `importe_neto`) VALUES
(1, 1, 'Preboda', '1.00', '250.00', '25.00', '275.00'),
(2, 1, 'Boda', '1.00', '500.00', '50.00', '550.00'),
(3, 2, 'Menu Adulto', '70.00', '100.00', '21.00', '121.00'),
(4, 2, 'Menu Infantil', '30.00', '50.00', '10.50', '60.50'),
(5, 3, 'Flores', '10.00', '20.00', '4.20', '24.20'),
(6, 3, 'Montaje y puesta en marcha', '2.00', '50.00', '10.50', '60.50'),
(7, 3, 'Orquesta', '1.00', '5.00', '1.05', '6.05'),
(8, 3, 'Menu Adulto', '40.00', '100.00', '21.00', '121.00'),
(9, 3, 'Menu Infantil', '10.00', '50.00', '10.50', '60.50'),
(10, 3, 'Oferta', '1.00', '-100.00', '-21.00', '-121.00'),
(11, 4, 'preboda', '5.00', '50.00', '5.00', '55.00'),
(12, 4, 'Boda', '7.00', '55.00', '5.50', '60.50');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedores`
--

CREATE TABLE `proveedores` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `direccion` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `poblacion` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `provincia` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `cp` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `telefono` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `movil` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT '',
  `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `password` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `cif` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `proveedores`
--

INSERT INTO `proveedores` (`id`, `nombre`, `direccion`, `poblacion`, `provincia`, `cp`, `telefono`, `movil`, `email`, `password`, `cif`) VALUES
(1, 'Proveedor de prueba', 'Calle Alicante', 'San Vicente del Raspeig', 'Alicante', '03690', '666000666', '666000666', 'proveedor@deprueba.info', '4f49454fb671c81b34dc23436070b1cd', 'B44225566'),
(2, 'Proveedor de prueba 2', 'Calle Doctor Marañon', 'San Vicente del Raspeig', 'Alicante', '03690', '666111222', '777777777', 'proveedor2deprueba2.info', '4f49454fb671c81b34dc23436070b1cd', 'B66776655');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedores_eventos_clientes`
--

CREATE TABLE `proveedores_eventos_clientes` (
  `id` int(11) NOT NULL,
  `ref_proveedor` int(11) DEFAULT NULL,
  `ref_cliente` int(11) DEFAULT NULL,
  `ref_evento` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `proveedores_eventos_clientes`
--

INSERT INTO `proveedores_eventos_clientes` (`id`, `ref_proveedor`, `ref_cliente`, `ref_evento`) VALUES
(1, 1, NULL, 1),
(2, NULL, 1, 1),
(3, NULL, 2, 1),
(4, 1, NULL, 2),
(5, NULL, 3, 2),
(6, NULL, 4, 2),
(7, 1, NULL, 3),
(8, NULL, 1, 3),
(9, NULL, 2, 3),
(10, 1, NULL, 4),
(11, NULL, 3, 4),
(12, NULL, 4, 4);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedores_servicios`
--

CREATE TABLE `proveedores_servicios` (
  `id` int(11) NOT NULL,
  `ref_proveedor` int(11) NOT NULL,
  `ref_servicio` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `proveedores_servicios`
--

INSERT INTO `proveedores_servicios` (`id`, `ref_proveedor`, `ref_servicio`) VALUES
(1, 1, 1),
(2, 1, 2),
(3, 1, 3),
(4, 1, 6),
(5, 2, 4),
(6, 2, 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `servicios`
--

CREATE TABLE `servicios` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `servicios`
--

INSERT INTO `servicios` (`id`, `nombre`) VALUES
(1, 'Restauración'),
(2, 'Hostelería'),
(3, 'Florista'),
(4, 'Montaje'),
(5, 'Decoración'),
(6, 'Asesoramiento');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tareas`
--

CREATE TABLE `tareas` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `fecha` int(11) DEFAULT NULL,
  `ref_evento` int(11) DEFAULT NULL,
  `realizada` tinyint(1) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `tareas`
--

INSERT INTO `tareas` (`id`, `nombre`, `fecha`, `ref_evento`, `realizada`) VALUES
(1, 'Buscar flores', 1560623006, 1, 1),
(3, 'Elegir flores', 1560684014, 1, 0),
(4, 'Contratar flores', 1560684031, 1, 0),
(5, 'Buscar catering', 1560688940, 1, 1),
(6, 'Elegir Catering', 1560688955, 1, 0),
(7, 'Contratar catering', 1560689000, 1, 0);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `clientes`
--
ALTER TABLE `clientes`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni_UNIQUE` (`dni`),
  ADD UNIQUE KEY `email_UNIQUE` (`email`);

--
-- Indices de la tabla `eventos`
--
ALTER TABLE `eventos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `mensajes`
--
ALTER TABLE `mensajes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_refevent_eventos_idx` (`ref_evento`),
  ADD KEY `fk_refcliente_clientes_idx` (`ref_cliente`),
  ADD KEY `fk_refproveedor_proveedores_idx` (`ref_proveedor`),
  ADD KEY `fk_refevent_mensajes_idx` (`ref_evento`),
  ADD KEY `fk_refcliente_mensajes_idx` (`ref_cliente`),
  ADD KEY `fk_refproveedor_mensajes_idx` (`ref_proveedor`);

--
-- Indices de la tabla `presupuestos`
--
ALTER TABLE `presupuestos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_refcliente_clientes` (`ref_cliente`),
  ADD KEY `fk_refproveedor_proveedores` (`ref_proveedor`);

--
-- Indices de la tabla `presupuestos_lineas`
--
ALTER TABLE `presupuestos_lineas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_refpresupuesto_presupuestos` (`ref_presupuesto`);

--
-- Indices de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `unique_email` (`email`),
  ADD UNIQUE KEY `unique_cif` (`cif`);

--
-- Indices de la tabla `proveedores_eventos_clientes`
--
ALTER TABLE `proveedores_eventos_clientes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_idcliente_clientes` (`ref_cliente`),
  ADD KEY `fk_idproveedor_proveedores` (`ref_proveedor`),
  ADD KEY `fk_idevento_eventos` (`ref_evento`);

--
-- Indices de la tabla `proveedores_servicios`
--
ALTER TABLE `proveedores_servicios`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_refproveedor_proveedores` (`ref_proveedor`),
  ADD KEY `fk_refservicio_servicios` (`ref_servicio`);

--
-- Indices de la tabla `servicios`
--
ALTER TABLE `servicios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `tareas`
--
ALTER TABLE `tareas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_refevento_tareas_idx` (`ref_evento`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `clientes`
--
ALTER TABLE `clientes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `eventos`
--
ALTER TABLE `eventos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `mensajes`
--
ALTER TABLE `mensajes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de la tabla `presupuestos`
--
ALTER TABLE `presupuestos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `presupuestos_lineas`
--
ALTER TABLE `presupuestos_lineas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `proveedores_eventos_clientes`
--
ALTER TABLE `proveedores_eventos_clientes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `proveedores_servicios`
--
ALTER TABLE `proveedores_servicios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `servicios`
--
ALTER TABLE `servicios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `tareas`
--
ALTER TABLE `tareas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `mensajes`
--
ALTER TABLE `mensajes`
  ADD CONSTRAINT `fk_refcliente_mensajes` FOREIGN KEY (`ref_cliente`) REFERENCES `clientes` (`id`),
  ADD CONSTRAINT `fk_refevento_mensajes` FOREIGN KEY (`ref_evento`) REFERENCES `eventos` (`id`),
  ADD CONSTRAINT `fk_refproveedor_mensajes` FOREIGN KEY (`ref_proveedor`) REFERENCES `proveedores` (`id`);

--
-- Filtros para la tabla `presupuestos`
--
ALTER TABLE `presupuestos`
  ADD CONSTRAINT `fk_refcliente_clientes` FOREIGN KEY (`ref_cliente`) REFERENCES `clientes` (`id`),
  ADD CONSTRAINT `kf_refproveedor_proveedores` FOREIGN KEY (`ref_proveedor`) REFERENCES `proveedores` (`id`);

--
-- Filtros para la tabla `presupuestos_lineas`
--
ALTER TABLE `presupuestos_lineas`
  ADD CONSTRAINT `fk_refpresupuesto_presupuestos` FOREIGN KEY (`ref_presupuesto`) REFERENCES `presupuestos` (`id`);

--
-- Filtros para la tabla `proveedores_eventos_clientes`
--
ALTER TABLE `proveedores_eventos_clientes`
  ADD CONSTRAINT `fk_refcliente_pec` FOREIGN KEY (`ref_cliente`) REFERENCES `clientes` (`id`),
  ADD CONSTRAINT `fk_refevento_pec` FOREIGN KEY (`ref_evento`) REFERENCES `eventos` (`id`),
  ADD CONSTRAINT `fk_refproveedor_pec` FOREIGN KEY (`ref_proveedor`) REFERENCES `proveedores` (`id`);

--
-- Filtros para la tabla `proveedores_servicios`
--
ALTER TABLE `proveedores_servicios`
  ADD CONSTRAINT `fk_refproveedor_proveedores` FOREIGN KEY (`ref_proveedor`) REFERENCES `proveedores` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_refservicio_servicios` FOREIGN KEY (`ref_servicio`) REFERENCES `servicios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `tareas`
--
ALTER TABLE `tareas`
  ADD CONSTRAINT `fk_refevento_tareas` FOREIGN KEY (`ref_evento`) REFERENCES `eventos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
