import { z } from 'zod';
import { validationMessages } from '@/utils/constants';

export const signUpFormDataSchema = z.object({
  userName: z
    .string()
    .nonempty(validationMessages.nonEmpty)
    .regex(/^[a-zA-Z0-9]*$/, { error: 'Must only contain letters and digits.' })
    .trim(),
  email: z.string().nonempty(validationMessages.nonEmpty).email().trim(),
  password: z
    .string()
    .nonempty(validationMessages.nonEmpty)
    .min(6, { message: 'Must be at least 6 characters.' })
    .trim(),
  passwordRepeat: z.string().nonempty(validationMessages.nonEmpty).trim(),
  acceptTerms: z.boolean(),
});

export type SignUpFormData = z.infer<typeof signUpFormDataSchema>;
