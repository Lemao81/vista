import { z } from 'zod';
import { errorCodes } from '@/utils/errors';

export const problemDetailsSchema = z.object({
  type: z.string(),
  title: z.string(),
  status: z.number(),
  errors: z.record(z.string(), z.array(z.string())).optional(),
  errorCode: z.enum([errorCodes.validationFailed]),
});

export const validationProblemDetailsSchema = problemDetailsSchema
  .extend({
    errors: z.record(z.string(), z.array(z.string())),
    errorCode: z.string(),
  })
  .refine((d) => d.errorCode === errorCodes.validationFailed);
