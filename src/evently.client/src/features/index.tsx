import { Suspense, lazy } from "react";
import * as React from "react";

const _HealthcheckPage = lazy(() => import("./healthcheck/healthcheck-page"));

export const HealthCheckPage: React.FC = () => (
    <Suspense>
        <_HealthcheckPage />
    </Suspense>
)