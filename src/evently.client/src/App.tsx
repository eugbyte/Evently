import "./App.css";
import { useState } from "react";
import { HealthCheckService } from "./features/healthcheck/_services";
import useAsyncEffect from "use-async-effect";
import { Container } from "typedi";

function App() {
	const healthCheckService = Container.get(HealthCheckService);
	const [status, setStatus] = useState("");
	useAsyncEffect(async () => {
		const statuses: string[] = await healthCheckService.getStatus();
		setStatus(statuses.join(", "));
	});
	return (
		<div>
			<p>{status}</p>
		</div>
	);
}

export default App;
